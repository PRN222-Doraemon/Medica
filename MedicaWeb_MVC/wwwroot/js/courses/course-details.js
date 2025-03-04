document.addEventListener('DOMContentLoaded', function () {
    const stars = document.querySelectorAll('.rating-input');
    const labels = document.querySelectorAll('.rating-label');

    // Xử lý hiệu ứng hover
    labels.forEach((label, index) => {
        // Xử lý khi hover vào ngôi sao
        label.addEventListener('mouseenter', () => {
            // Tô màu cho tất cả các ngôi sao từ đầu đến vị trí đang hover
            for (let i = 0; i <= index; i++) {
                labels[i].classList.add('hover');
            }
        });

        // Xử lý khi rời chuột khỏi ngôi sao
        label.addEventListener('mouseleave', () => {
            // Xóa class hover khỏi tất cả các ngôi sao
            labels.forEach(l => l.classList.remove('hover'));
        });

        // Xử lý khi click vào ngôi sao
        label.addEventListener('click', () => {
            // Xóa class active khỏi tất cả các ngôi sao
            labels.forEach(l => l.classList.remove('active'));

            // Tô màu cho tất cả các ngôi sao từ đầu đến vị trí đang click
            for (let i = 0; i <= index; i++) {
                labels[i].classList.add('active');
            }

            // Chọn radio button tương ứng
            stars[index].checked = true;
        });
    });

    // Xử lý khi form được submit hoặc radio button được chọn trực tiếp
    stars.forEach((star, index) => {
        star.addEventListener('change', function () {
            // Xóa class active khỏi tất cả các ngôi sao
            labels.forEach(l => l.classList.remove('active'));

            // Tô màu cho tất cả các ngôi sao từ đầu đến vị trí được chọn
            for (let i = 0; i <= index; i++) {
                labels[i].classList.add('active');
            }
        });
    });

});

function toggleReviews() {
    let hiddenReviews = document.querySelectorAll(".hidden-review");
    let allReviews = document.querySelectorAll(".border-bottom.pb-4.mb-4");
    let viewMoreBtn = document.getElementById("viewMoreReviews");
    let visibleCount = 0; // Số lượng review đã hiển thị thêm mỗi lần
    let maxVisible = 3; // Số review hiển thị thêm mỗi lần

    if (viewMoreBtn.dataset.state === "expand") {
        let count = 0;
        hiddenReviews.forEach((review) => {
            if (count < maxVisible && review.classList.contains("hidden-review")) {
                review.classList.remove("hidden-review");
                count++;
                visibleCount++;
            }
        });

        // Nếu đã hiển thị hết review -> Đổi thành "Show less"
        if (document.querySelectorAll(".hidden-review").length === 0) {
            viewMoreBtn.innerHTML = 'Show less <i class="bi bi-chevron-up ms-1"></i>';
            viewMoreBtn.dataset.state = "collapse";
        }
    } else {
        // Ẩn tất cả review, chỉ giữ lại 3 cái đầu tiên
        allReviews.forEach((review, index) => {
            if (index >= maxVisible) {
                review.classList.add("hidden-review");
            }
        });

        viewMoreBtn.innerHTML = 'Show more reviews <i class="bi bi-chevron-down ms-1"></i>';
        viewMoreBtn.dataset.state = "expand";
    }
}