// wwwroot/js/site.js

// --- SCROLL MANAGEMENT ---

/**
 * Saves the current scroll position to localStorage to prevent jumping after page reload
 * @param {string} pageId - The ID of the current page being edited
 */
function saveScroll(pageId) {
    localStorage.setItem('editor_scroll_' + pageId, window.scrollY);
}

/**
 * Restores the scroll position when the page finishes loading
 */
document.addEventListener('DOMContentLoaded', function () {
    const editorElement = document.querySelector('[data-page-id]');
    if (editorElement) {
        const pageId = editorElement.dataset.pageId;
        const pos = localStorage.getItem('editor_scroll_' + pageId);
        if (pos) {
            window.scrollTo({ top: parseInt(pos), behavior: 'instant' });
            localStorage.removeItem('editor_scroll_' + pageId);
        }
    }
});

// --- BLOCK MANAGEMENT ACTIONS ---

/**
 * Creates and submits a form to add a new block to the page
 */
window.executeAddBlock = function (pageId, type, parentId) {
    saveScroll(pageId);

    const form = document.createElement('form');
    form.method = 'POST';
    form.action = '/Manager/Manager/AddBlock';

    const data = {
        'pageId': pageId,
        'blockType': type,
        'parentId': parentId || '',
        '__RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
    };

    for (const key in data) {
        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = key;
        input.value = data[key];
        form.appendChild(input);
    }
    document.body.appendChild(form);
    form.submit();
};

/**
 * Creates and submits a form to move a block's position (Up/Down)
 */
window.executeMoveBlock = function (pageId, blockId, direction) {
    saveScroll(pageId);

    const form = document.createElement('form');
    form.method = 'POST';
    form.action = '/Manager/Manager/MoveBlock';

    const data = {
        'blockId': blockId,
        'direction': direction,
        'pageId': pageId,
        '__RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
    };

    for (const key in data) {
        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = key;
        input.value = data[key];
        form.appendChild(input);
    }
    document.body.appendChild(form);
    form.submit();
};

/**
 * Handles block deletion with user confirmation
 */
window.executeDeleteBlock = function (pageId, blockId) {
    if (!confirm('Are you sure you want to delete this block?')) return;
    saveScroll(pageId);

    const form = document.createElement('form');
    form.method = 'POST';
    form.action = '/Manager/Manager/DeleteBlock';

    const data = {
        'blockId': blockId,
        'pageId': pageId,
        '__RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
    };

    for (const key in data) {
        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = key;
        input.value = data[key];
        form.appendChild(input);
    }
    document.body.appendChild(form);
    form.submit();
};

/**
 * Universal action handler for moving or deleting via fetch (alternative method)
 */
window.executeAction = function (pageId, blockId, direction, action) {
    if (action === 'DeleteBlock' && !confirm('Are you sure?')) return;

    const formData = new FormData();
    formData.append('pageId', pageId);
    formData.append('blockId', blockId);
    if (direction) formData.append('direction', direction);
    formData.append('__RequestVerificationToken', document.querySelector('input[name="__RequestVerificationToken"]').value);

    fetch(`/Manager/Manager/${action}`, { method: 'POST', body: formData })
        .then(r => r.ok ? window.location.reload() : alert('Action error'));
};

// --- MEDIA PICKER LOGIC ---

let mediaPickerTarget  = null;
let mediaPickerWrapper = null;  // wrapperId del preview (opcional)

/**
 * Abre el media picker popup.
 * @param {string} targetInputId  - ID del <input> donde se guarda la URL
 * @param {string} [wrapperId]    - ID del div contenedor del preview (opcional)
 */
window.openMediaPicker = function (targetInputId, wrapperId) {
    mediaPickerTarget  = targetInputId;
    mediaPickerWrapper = wrapperId || null;

    const width = 1100;
    const height = 700;
    const left = (screen.width - width) / 2;
    const top  = (screen.height - height) / 2;

    window.open('/Manager/Media/Index?picker=true', 'mediaPicker',
        `width=${width},height=${height},left=${left},top=${top},scrollbars=yes`);
};

/**
 * Limpia un ImageField: borra el valor del input y oculta el preview.
 * @param {string} inputId   - ID del <input>
 * @param {string} wrapperId - ID del div wrapper del preview
 */
window.clearImageField = function (inputId, wrapperId) {
    const input = document.getElementById(inputId);
    if (input) input.value = '';

    const wrapper = wrapperId ? document.getElementById(wrapperId) : null;
    if (wrapper) wrapper.classList.add('hidden');
};

/**
 * Escucha mensajes del popup media picker
 */
window.addEventListener('message', function (event) {
    if (event.data.type === 'media-selected') {
        const input = document.getElementById(mediaPickerTarget);
        if (input) {
            input.value = event.data.url;

            // Actualizar preview del wrapper (nuevo sistema)
            if (mediaPickerWrapper) {
                const wrapper = document.getElementById(mediaPickerWrapper);
                if (wrapper) {
                    wrapper.classList.remove('hidden');
                    const preview = document.getElementById(mediaPickerWrapper + '-preview');
                    if (preview) preview.src = event.data.url;
                }
            } else {
                // Fallback: sistema antiguo con .image-field-container
                const container = input.closest('.image-field-container');
                const imgPreview = container?.querySelector('img');
                if (imgPreview) imgPreview.src = event.data.url;
            }

            // Evento para Alpine.js
            const customEvent = new CustomEvent('media-selected-final', {
                detail: { url: event.data.url, target: mediaPickerTarget }
            });
            window.dispatchEvent(customEvent);
        }
        mediaPickerTarget  = null;
        mediaPickerWrapper = null;
    }
});

// --- MARKDOWN BLOCK RENDERER ---

/**
 * Renders the editor UI for Markdown blocks
 */
window.renderMarkdownBlock = function (container, jsonData) {
    let data = {};
    try {
        data = JSON.parse(jsonData || '{}');
    } catch (e) {
        console.warn('Error parsing MarkdownBlock JSON:', e);
    }

    container.innerHTML = '';

    // Markdown Content Field
    const contentGroup = document.createElement('div');
    contentGroup.className = 'mb-4';

    const contentLabel = document.createElement('label');
    contentLabel.className = 'block text-sm font-medium text-gray-700 mb-1';
    contentLabel.textContent = 'Markdown Content';
    contentLabel.setAttribute('for', 'markdown-content');
    contentGroup.appendChild(contentLabel);

    const contentTextarea = document.createElement('textarea');
    contentTextarea.id = 'markdown-content';
    // Removed 'name' to prevent accidental double-submission
    contentTextarea.value = data.Content?.Value || '';
    contentTextarea.placeholder = 'Write in Markdown...';
    contentTextarea.className = 'w-full font-mono text-sm border border-gray-300 rounded p-2 min-h-[200px] focus:ring-2 focus:ring-blue-500 focus:border-blue-500';
    contentTextarea.setAttribute('x-model', 'markdownContent');
    contentGroup.appendChild(contentTextarea);

    container.appendChild(contentGroup);

    // Sanitization Toggle (BoolField)
    const boolGroup = document.createElement('div');
    boolGroup.className = 'mb-4 flex items-center gap-2';

    const boolCheckbox = document.createElement('input');
    boolCheckbox.type = 'checkbox';
    boolCheckbox.id = 'allow-raw-html';
    boolCheckbox.checked = data.AllowRawHtml?.Value === 'true' || data.AllowRawHtml?.Value === true;
    boolCheckbox.value = 'true';
    boolCheckbox.className = 'h-4 w-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500';
    boolCheckbox.setAttribute('x-model', 'allowRawHtml');

    const boolLabel = document.createElement('label');
    boolLabel.setAttribute('for', 'allow-raw-html');
    boolLabel.className = 'text-sm text-gray-700';
    boolLabel.textContent = 'Allow Unsanitized HTML';

    boolGroup.appendChild(boolCheckbox);
    boolGroup.appendChild(boolLabel);
    container.appendChild(boolGroup);
};

/**
 * Prepares the JSON string for Markdown blocks before saving
 */
window.prepareMarkdownBlockJson = function (container) {
    const contentValue = container.querySelector('#markdown-content')?.value || '';
    const allowRawValue = container.querySelector('#allow-raw-html')?.checked ? 'true' : 'false';

    return JSON.stringify({
        Content: { Value: contentValue },
        AllowRawHtml: { Value: allowRawValue }
    });
};

// --- ADVANCED BLOCK RENDERER ---

/**
 * Renders Advanced/Hero block UI with Title and Style settings
 */
window.renderAdvancedBlock = function (container, jsonData) {
    let data = {};
    try { data = JSON.parse(jsonData || '{}'); } catch (e) { }

    // Removed 'name' attributes from inputs to solve the "comma-separated duplicate" bug
    container.innerHTML = `
        <div class="block-editor-grid space-y-4">
            <div class="field-group border p-4 rounded bg-white">
                <label class="block text-sm font-medium text-gray-700 mb-1">Title</label>
                <input type="text" id="title-val" value="${data.Title?.Value || ''}" class="w-full p-2 border rounded">
                
                <div class="style-controls flex gap-4 mt-3">
                    <div class="flex-1">
                        <label class="block text-xs text-gray-500">Title Color</label>
                        <input type="color" id="title-color" value="${data.TitleColor?.Value || '#000000'}" class="h-10 w-full cursor-pointer">
                    </div>
                    <div class="flex-1">
                        <label class="block text-xs text-gray-500">Title Size</label>
                        <select id="title-size" class="w-full p-2 border rounded">
                            <option value="text-xl" ${data.TitleSize?.Value === 'text-xl' ? 'selected' : ''}>Large</option>
                            <option value="text-3xl" ${data.TitleSize?.Value === 'text-3xl' ? 'selected' : ''}>Giant</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>
    `;
};

/**
 * Prepares clean JSON for advanced blocks, preventing data loss
 */
window.prepareAdvancedBlockJson = function (container) {
    return JSON.stringify({
        Title: { Value: container.querySelector('#title-val').value },
        TitleColor: { Value: container.querySelector('#title-color').value },
        TitleSize: { Value: container.querySelector('#title-size').value }
    });
};

// --- PAGE SETTINGS ---

/**
 * Saves the page background color via AJAX
 */
function savePageColor() {
    const colorInput = document.getElementById('pageColorInput');
    if (!colorInput) return;

    const color = colorInput.value;
    const pageId = document.querySelector('[data-page-id]')?.dataset.pageId;
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    const formData = new FormData();
    formData.append('pageId', pageId);
    formData.append('backgroundColor', color);
    formData.append('__RequestVerificationToken', token);

    fetch('/Manager/Manager/SavePageSettings', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) window.location.reload();
        else alert('Error saving page color');
    });
}

// --- CHAT LOCAL STORAGE (30-day persistence) ---

window.chatStorage = {
    save: function(chatId, messagesJson) {
        const key = 'brixcms_chat_' + chatId;
        const data = { messages: messagesJson, savedAt: new Date().toISOString() };
        localStorage.setItem(key, JSON.stringify(data));
    },
    load: function(chatId) {
        const key = 'brixcms_chat_' + chatId;
        const raw = localStorage.getItem(key);
        if (!raw) return null;
        try {
            const data = JSON.parse(raw);
            const saved = new Date(data.savedAt);
            const now = new Date();
            const daysDiff = (now - saved) / (1000 * 60 * 60 * 24);
            if (daysDiff > 30) {
                localStorage.removeItem(key);
                return null;
            }
            return data.messages;
        } catch (e) {
            localStorage.removeItem(key);
            return null;
        }
    },
    clear: function(chatId) {
        const key = 'brixcms_chat_' + chatId;
        localStorage.removeItem(key);
    }
};

window.chatLockBody = function(lock) {
    if (lock) {
        document.body.style.overflow = 'hidden';
        document.body.style.position = 'fixed';
        document.body.style.width = '100%';
        document.body.style.height = '100%';
    } else {
        document.body.style.overflow = '';
        document.body.style.position = '';
        document.body.style.width = '';
        document.body.style.height = '';
    }
};
