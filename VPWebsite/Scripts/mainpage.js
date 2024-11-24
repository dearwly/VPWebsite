$(document).ready(function () {
    const $avatar = $('.avatar-img');
    const $hoverMenu = $('.hover-menu');

    // hover menu
    $avatar.hover(
        function () { // mouseenter
            $hoverMenu.addClass('active');
        },
        function () { // mouseleave
            setTimeout(() => {
                if (!$hoverMenu.is(':hover')) {
                    $hoverMenu.removeClass('active');
                }
            }, 200);
        }
    );

    // cursor 
    $hoverMenu.on('mouseleave', function () {
        $hoverMenu.removeClass('active');
    });

    // click upload to choose file
    $("#avatarBtn").on("click", function () {
        $("#avatarFile").click(); 
    });

    // auto upload
    $("#avatarFile").on("change", function () {
        var formData = new FormData($("#uploadAvatarForm")[0]);

        $.ajax({
            url: "/Upload/UploadAvatar",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    alert(response.message); 
                    location.reload();
                } else {
                    alert(response.message); 
                }
            },
            error: function () {
                alert("Upload failed, please try again later!");
            }
        });
    });
});
