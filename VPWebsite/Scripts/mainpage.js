const avatar = document.querySelector('.avatar-img');
const hoverMenu = document.querySelector('.hover-menu');

// 鼠标悬浮显示悬浮菜单
avatar.addEventListener('mouseenter', () => {
    hoverMenu.classList.add('active');
});

// 鼠标移出隐藏悬浮菜单
avatar.addEventListener('mouseleave', () => {
    setTimeout(() => {
        if (!hoverMenu.matches(':hover')) {
            hoverMenu.classList.remove('active');
        }
    }, 200);
});

// 鼠标从菜单移开隐藏菜单
hoverMenu.addEventListener('mouseleave', () => {
    hoverMenu.classList.remove('active');
});