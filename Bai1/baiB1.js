(() => {
    'use strict';

    const $ = (id) => document.getElementById(id);

    const updateProfileBtn = $('updateProfileBtn');
    const displayName = $('displayName');
    const displayMssv = $('displayMssv');
    const displayClass = $('displayClass');

    const interestInput = $('interestInput');
    const addInterestBtn = $('addInterestBtn');
    const interestList = $('interestList');

    const toggleCodeBtn = $('toggleCodeBtn');
    const codeBlock = $('codeBlock');

    const themeToggleBtn = $('themeToggleBtn');

    function promptNonEmpty(message) {
        while (true) {
            const raw = window.prompt(message);
            if (raw === null) return null; // user cancel

            const value = raw.trim();
            if (value.length > 0) return value;

            window.alert('Không được để trống. Vui lòng nhập lại.');
        }
    }

    function updateProfile() {
        const name = promptNonEmpty('Nhập tên mới:');
        if (name === null) return;

        const mssv = promptNonEmpty('Nhập MSSV mới:');
        if (mssv === null) return;

        const cls = promptNonEmpty('Nhập Lớp mới:');
        if (cls === null) return;

        if (displayName) displayName.textContent = name;
        if (displayMssv) displayMssv.textContent = mssv;
        if (displayClass) displayClass.textContent = cls;
    }

    if (updateProfileBtn) {
        updateProfileBtn.addEventListener('click', updateProfile);
    }

    function addInterest() {
        if (!interestInput || !interestList) return;

        const value = interestInput.value.trim();
        if (value.length === 0) {
            window.alert('Sở thích không được rỗng.');
            interestInput.focus();
            return;
        }

        const li = document.createElement('li');
        li.textContent = value;
        interestList.appendChild(li);

        interestInput.value = '';
        interestInput.focus();
    }

    if (addInterestBtn) {
        addInterestBtn.addEventListener('click', addInterest);
    }

    if (interestInput) {
        interestInput.addEventListener('keydown', (e) => {
            if (e.key === 'Enter') addInterest();
        });
    }

    let isCodeVisible = true;

    function setCodeVisible(next) {
        isCodeVisible = next;
        if (!codeBlock || !toggleCodeBtn) return;

        codeBlock.classList.toggle('hidden', !isCodeVisible);
        toggleCodeBtn.textContent = isCodeVisible ? 'Ẩn mã' : 'Hiện mã';
    }

    if (codeBlock && toggleCodeBtn) {
        // Initialize from current DOM state
        isCodeVisible = !codeBlock.classList.contains('hidden');
        setCodeVisible(isCodeVisible);

        toggleCodeBtn.addEventListener('click', () => {
            setCodeVisible(!isCodeVisible);
        });
    }

    function applyTheme(theme) {
        const isDark = theme === 'dark';
        document.body.classList.toggle('dark', isDark);

        if (themeToggleBtn) {
            themeToggleBtn.textContent = isDark ? 'Chế độ sáng' : 'Chế độ tối';
        }
    }

    function toggleTheme() {
        const isDark = document.body.classList.contains('dark');
        applyTheme(isDark ? 'light' : 'dark');
        try {
            localStorage.setItem('theme', document.body.classList.contains('dark') ? 'dark' : 'light');
        } catch (_) {
            // ignore storage error
        }
    }

    // Load theme on first visit
    try {
        const saved = localStorage.getItem('theme');
        applyTheme(saved === 'dark' ? 'dark' : 'light');
    } catch (_) {
        applyTheme('light');
    }

    if (themeToggleBtn) {
        themeToggleBtn.addEventListener('click', toggleTheme);
    }
})();

