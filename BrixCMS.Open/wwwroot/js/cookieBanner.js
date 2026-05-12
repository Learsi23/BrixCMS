


document.addEventListener("DOMContentLoaded", function () {
    const decision = localStorage.getItem("cookieConsent");
    const banner = document.getElementById("cookieBanner");
    if (!decision && banner) {
        banner.style.display = "block";
    }
});

function gestionarCookies(aceptado) {
    localStorage.setItem("cookieConsent", aceptado ? "accepted" : "rejected");
    const banner = document.getElementById("cookieBanner");
    if (banner) banner.style.display = "none";
}