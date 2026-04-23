// ==================== Script.js - BrixCMS ====================
// ⚠️ SIN 'export' - funciones globales en window para Blazor Server
// ⚠️ Los 'export function' del archivo anterior eran incompatibles con
//    JS.InvokeAsync — Blazor espera funciones en window, no módulos ES6.
// ✅ Todo el código es plano y funciona sin dependencias externas.
// =============================================================


// ==================== NAVBAR ====================

// Aplica delays escalonados a elementos con clase .fade-in
// para que aparezcan uno a uno en lugar de todos a la vez.
window.addDelay = function () {
    document.querySelectorAll('.fade-in').forEach(function (img, index) {
        img.style.animationDelay = `${index * 0.2}s`;
    });
};

// Controla qué navbar se muestra según el scroll:
// - fullscreen-nav: visible al tope de la página
// - simplified-nav: visible al bajar más de 100px
function handleScroll() {
    const fullscreenNav = document.getElementById('fullscreen-nav');
    const simplifiedNav = document.getElementById('simplified-nav');

    // Protección: si la página no tiene navbar, no hacer nada
    if (!fullscreenNav || !simplifiedNav) return;

    const currentScroll = window.pageYOffset;

    if (currentScroll > 100) {
        fullscreenNav.style.opacity = '0';
        fullscreenNav.style.visibility = 'hidden';
        simplifiedNav.style.opacity = '1';
        simplifiedNav.style.visibility = 'visible';
    } else {
        fullscreenNav.style.opacity = '1';
        fullscreenNav.style.visibility = 'visible';
        simplifiedNav.style.opacity = '0';
        simplifiedNav.style.visibility = 'hidden';
    }
}

// Listener de scroll con throttle para no ejecutar handleScroll
// en cada pixel — solo cada ~16ms (aprox. 60fps).
// El viejo código tenía isTicking dentro del callback (bug),
// aquí está correctamente en el scope externo.
(function () {
    var isTicking = false;

    window.addEventListener('scroll', function () {
        if (!isTicking) {
            isTicking = true;
            setTimeout(function () {
                handleScroll();
                isTicking = false;
            }, 16);
        }
    }, { passive: true }); // passive: true mejora rendimiento en móvil

    // Ejecutar al cargar para establecer el estado inicial del navbar
    handleScroll();
})();


// ==================== MENU MOBILE ====================

// Alterna la visibilidad del menú móvil y la animación del botón hamburguesa.
// Llamado desde el HTML con onclick="toggleMenu()"
window.toggleMenu = function () {
    const mobileMenu = document.getElementById('mobile-menu');
    const hamburger = document.querySelector('.hamburger-menu');

    // Protección: salir si los elementos no existen en esta página
    if (!mobileMenu || !hamburger) return;

    hamburger.classList.toggle('change');
    mobileMenu.style.display = mobileMenu.style.display === 'none' ? 'block' : 'none';
};


// ==================== CHAT: AUTO-SCROLL ====================

// Observa cambios en el contenedor de mensajes del chat y hace
// scroll automático hacia abajo cuando llega contenido nuevo.
// Retorna un objeto { dispose } para que Blazor pueda limpiarlo
// cuando el componente se destruye (ChatMessageList.DisposeAsync).
window.initAutoScroll = function (container) {
    if (!container) {
        return { dispose: function () { } };
    }

    const observer = new MutationObserver(function () {
        try {
            container.scrollTop = container.scrollHeight;
        } catch (e) {
            // Ignorar errores si el elemento ya no existe en el DOM
        }
    });

    observer.observe(container, {
        childList: true,  // detectar mensajes nuevos
        subtree: true     // detectar cambios dentro de los mensajes (streaming)
    });

    return {
        dispose: function () {
            try {
                observer.disconnect();
            } catch (e) { }
        }
    };
};


// ==================== CHAT: TEXTAREA AUTO-RESIZE ====================

// Hace que el textarea del input crezca verticalmente conforme
// el usuario escribe, hasta el máximo definido en CSS (max-h-32).
// Retorna { dispose } para limpieza desde ChatInput.DisposeAsync.
window.initTextareaAutoResize = function (textarea) {
    if (!textarea) {
        return { dispose: function () { } };
    }

    function handleInput() {
        // Reset a auto primero para recalcular correctamente hacia abajo
        this.style.height = 'auto';
        this.style.height = this.scrollHeight + 'px';
    }

    textarea.addEventListener('input', handleInput);

    // Ejecutar una vez al inicio por si ya hay texto (ej. al volver a la página)
    handleInput.call(textarea);

    return {
        dispose: function () {
            try {
                textarea.removeEventListener('input', handleInput);
            } catch (e) { }
        }
    };
};


// ==================== CHAT: BLOQUEAR ENTER EN TEXTAREA ====================

// Previene que Enter agregue una línea nueva en el textarea del chat.
// Blazor Server no puede hacer preventDefault desde C# porque el evento
// ya fue procesado en el cliente — por eso se maneja aquí en JS.
// Shift+Enter sigue funcionando para saltos de línea intencionales.
document.addEventListener('keydown', function (e) {
    if (e.target.matches('textarea') && e.key === 'Enter' && !e.shiftKey) {
        e.preventDefault();
    }
}, true); // capture: true para interceptar antes que Blazor


// ==================== INICIALIZACIONES AL CARGAR ====================

window.addEventListener('load', function () {
    // Aplicar delays a animaciones fade-in si existen en la página
    window.addDelay();
});


// ==================== NAMESPACE OPCIONAL ====================

// Alias organizacional — no reemplaza las funciones globales,
// solo las agrupa por si se quieren llamar como BrixCMS.Chat.X
window.BrixCMS = window.BrixCMS || {};
window.BrixCMS.Chat = {
    initAutoScroll: window.initAutoScroll,
    initTextareaAutoResize: window.initTextareaAutoResize
};