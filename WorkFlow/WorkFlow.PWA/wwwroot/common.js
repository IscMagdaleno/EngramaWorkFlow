
window.scrollToBottom = function (element) {
    if (element) {
        element.scrollTop = element.scrollHeight;
    }
};

window.scrollToBottomSmooth = function (element) {
    if (element) {
        element.scrollTo({
            top: element.scrollHeight,
            behavior: 'smooth'
        });
    }
};

// Función para detectar si el usuario está en el fondo del chat
window.isScrolledToBottom = function (element) {
    if (element) {
        return element.scrollHeight - element.clientHeight <= element.scrollTop + 1;
    }
    return false;
};