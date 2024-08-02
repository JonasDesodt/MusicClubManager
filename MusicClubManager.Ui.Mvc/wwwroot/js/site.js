const btnMenu = document.querySelector('header > div > button');
const body = document.querySelector('body');
btnMenu.addEventListener('click', () => {
    if (body.classList.contains("menu")) {
        body.classList.remove("menu");
    } else {
        body.classList.add("menu")
    }
});