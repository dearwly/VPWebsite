$(document).ready(function () {
    const $avatar = $('.avatar-img');
    const $hoverMenu = $('.hover-menu');

    // 鼠标悬浮显示悬浮菜单
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

    // 鼠标从菜单移开隐藏菜单
    $hoverMenu.on('mouseleave', function () {
        $hoverMenu.removeClass('active');
    });

    // 点击“上传头像”按钮触发文件选择框
    $("#avatarBtn").on("click", function () {
        $("#avatarFile").click(); // 触发隐藏的文件选择框
    });

    // 文件选择后自动上传
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
                    alert(response.message); // 显示成功提示
                    location.reload(); // 刷新页面
                } else {
                    alert(response.message); // 显示错误提示
                }
            },
            error: function () {
                alert("上传失败，请稍后再试！");
            }
        });
    });
});
