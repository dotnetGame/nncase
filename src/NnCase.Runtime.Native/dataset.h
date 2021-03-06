#pragma once
#include <filesystem>
#include <functional>
#include <optional>
#include <string>
#include <string_view>
#include <xtensor/xarray.hpp>
#include <xtensor/xshape.hpp>

namespace nncase
{
namespace data
{
    template <class T>
    struct data_batch
    {
        xt::xarray<T> tensor;
        xtl::span<const std::filesystem::path> filenames;
    };

    class dataset
    {
    public:
        template <class T>
        class iterator
        {
        public:
            using iterator_category = std::forward_iterator_tag;
            using value_type = data_batch<T>;
            using pointer = data_batch<T> *;
            using reference = data_batch<T> &;

            bool operator==(const iterator &rhs) const noexcept { return from_ == rhs.from_; }
            bool operator!=(const iterator &rhs) const noexcept { return from_ != rhs.from_; }

            iterator &operator++()
            {
                value_ = dataset_->batch<T>(from_ + dataset_->batch_size());
                if (value_)
                    from_ += dataset_->batch_size();
                else
                    *this = dataset_->end<T>();
                return *this;
            }

            iterator &operator=(const iterator &rhs)
            {
                value_ = rhs.value_;
                from_ = rhs.from_;
                return *this;
            }

            data_batch<T> &operator*()
            {
                if (value_)
                    return *value_;

                throw std::runtime_error("Invalid datast iterator");
            }

            data_batch<T> *operator->()
            {
                if (value_)
                    return &value_.value();

                throw std::runtime_error("Invalid datast iterator");
            }

        private:
            friend class dataset;

            iterator(dataset &dataset, size_t from)
                : dataset_(&dataset), from_(from), value_(dataset.batch<T>(from))
            {
            }

            dataset *dataset_;
            size_t from_;
            std::optional<data_batch<T>> value_;
        };

        dataset(const std::filesystem::path &path, std::function<bool(const std::filesystem::path &)> file_filter, xt::dynamic_shape<size_t> input_shape, float mean, float std);

        template <class T>
        iterator<T> begin()
        {
            return { *this, 0 };
        }

        template <class T>
        iterator<T> end()
        {
            return { *this, filenames_.size() };
        }

        size_t batch_size() const noexcept { return input_shape_[0]; }

    protected:
        virtual void process(const std::vector<uint8_t> &src, float *dest, const xt::dynamic_shape<size_t> &shape) = 0;
        virtual void process(const std::vector<uint8_t> &src, uint8_t *dest, const xt::dynamic_shape<size_t> &shape) = 0;

    private:
        template <class T>
        std::optional<data_batch<T>> batch(size_t from)
        {
            if (from + batch_size() <= filenames_.size())
            {
                size_t start = from;

                xt::xarray<T> batch(input_shape_);
                for (auto &&sample_view : xt::split(batch, batch_size()))
                {
                    auto view = xt::squeeze(sample_view, 0);
                    auto file = read_file(filenames_[from++]);
                    process(file, view.data(), view.shape());
                }

                xtl::span<const std::filesystem::path> filenames(filenames_.data() + start, filenames_.data() + from);

                return data_batch<T> { std::move(batch), filenames };
            }

            return {};
        }

    private:
        std::vector<std::filesystem::path> filenames_;
        xt::dynamic_shape<size_t> input_shape_;
    };

    class image_dataset : public dataset
    {
    public:
        image_dataset(const std::filesystem::path &path, xt::dynamic_shape<size_t> input_shape, float mean, float std);

    protected:
        void process(const std::vector<uint8_t> &src, float *dest, const xt::dynamic_shape<size_t> &shape) override;
        void process(const std::vector<uint8_t> &src, uint8_t *dest, const xt::dynamic_shape<size_t> &shape) override;
    };
}
}
