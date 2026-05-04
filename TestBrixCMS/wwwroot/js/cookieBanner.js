


document.addEventListener("DOMContentLoaded", function () {
    // Verificamos si ya existe una decisión guardada en el navegador
    const decision = localStorage.getItem("cookieConsent");

    // Si no ha decidido nada aún, mostramos el banner
    if (!decision) {
        document.getElementById("cookieBanner").style.display = "block";
    }
});

function gestionarCookies(aceptado) {
    if (aceptado) {
        // El usuario aceptó: Guardamos la decisión
        localStorage.setItem("cookieConsent", "aceptado");
        console.log("Cookies aceptadas.");
        // Aquí podrías disparar scripts de Google Analytics si los tuvieras
    } else {
        // El usuario rechazó: Guardamos que no quiere cookies
        localStorage.setItem("cookieConsent", "rechazado");
        console.log("Cookies rechazadas.");
    }

    // Ocultamos el banner con una transición simple
    document.getElementById("cookieBanner").style.display = "none";
}