$(document).ready(() => {
    $('#Image').on('change', function () {
        const file = this.files[0];
        if (file) {
            const url = URL.createObjectURL(file);
            $('#product-preview').attr('src', url).removeClass("d-none");;
        }
    });
});