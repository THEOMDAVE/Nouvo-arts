let cropper;
let activeInput;
let modalEl;
let modal;

document.addEventListener("DOMContentLoaded", () => {

    modalEl = document.getElementById("globalCropModal");

    if (!modalEl) return; // 👈 modal not on page

    modal = new bootstrap.Modal(modalEl);

    modalEl.addEventListener('shown.bs.modal', function () {

        const img = document.getElementById("globalCropImage");

        cropper = new Cropper(img, {
            viewMode: 1,
            autoCropArea: 1,
            cropBoxResizable: false,

            ready() {
                cropper.setCropBoxData({
                    height: 250,
                    width: (window.innerWidth - 36) / 4
                });
            }
        });
    });

    modalEl.addEventListener('hidden.bs.modal', function () {
        cropper?.destroy();
    });

    document.getElementById("globalCropBtn")?.addEventListener("click", function () {

        const canvas = cropper.getCroppedCanvas({
            height: 250
        });

        canvas.toBlob(function (blob) {

            const file = new File([blob], "cropped.png", { type: "image/png" });

            const dt = new DataTransfer();
            dt.items.add(file);

            activeInput.files = dt.files;

            modal.hide();
        });
    });

});

document.addEventListener("change", function (e) {

    if (!e.target.classList.contains("image-cropper")) return;

    activeInput = e.target;

    const file = activeInput.files[0];
    if (!file) return;

    const reader = new FileReader();

    reader.onload = function () {
        document.getElementById("globalCropImage").src = reader.result;
        modal.show();
    };

    reader.readAsDataURL(file);
});
