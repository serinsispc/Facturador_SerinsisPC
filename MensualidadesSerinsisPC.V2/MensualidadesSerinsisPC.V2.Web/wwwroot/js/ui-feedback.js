window.ms2Alerts = {
    success: function (title, text) {
        return Swal.fire({
            icon: "success",
            title: title,
            text: text,
            confirmButtonColor: "#0f6cbd"
        });
    },
    error: function (title, text) {
        return Swal.fire({
            icon: "error",
            title: title,
            text: text,
            confirmButtonColor: "#c62828"
        });
    },
    warning: function (title, text) {
        return Swal.fire({
            icon: "warning",
            title: title,
            text: text,
            confirmButtonColor: "#ed6c02"
        });
    },
    info: function (title, text) {
        return Swal.fire({
            icon: "info",
            title: title,
            text: text,
            confirmButtonColor: "#0f6cbd"
        });
    },
    confirm: async function (title, text, confirmText, cancelText) {
        const result = await Swal.fire({
            icon: "question",
            title: title,
            text: text,
            showCancelButton: true,
            confirmButtonText: confirmText,
            cancelButtonText: cancelText,
            confirmButtonColor: "#0f6cbd",
            cancelButtonColor: "#7a869a"
        });

        return result.isConfirmed === true;
    }
};
