// تبديل رؤية كلمة المرور
document.querySelector('.toggle-password').addEventListener('click', function () {
    const passwordInput = document.getElementById('password');
    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput.setAttribute('type', type);
});

// إرسال النموذج
document.getElementById('loginForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    // أضف منطق تسجيل الدخول هنا
    console.log('محاولة تسجيل الدخول:', { username, password });

    // مثال على التحقق
    if (username && password) {
        alert('تم تسجيل الدخول بنجاح!');
    } else {
        alert('الرجاء إدخال اسم المستخدم وكلمة المرور');
    }
});

// تبديل اللغة
const langButtons = document.querySelectorAll('.lang-btn');
langButtons.forEach(btn => {
    btn.addEventListener('click', function () {
        langButtons.forEach(b => b.classList.remove('active'));
        this.classList.add('active');

        // هنا يمكنك إضافة منطق تبديل اللغة
        if (this.textContent === 'EN') {
            window.location.href = 'index-en.html'; // الصفحة الإنجليزية
        }
    });
});

// إضافة تأثيرات الحركة للحقول
const inputs = document.querySelectorAll('input');
inputs.forEach(input => {
    input.addEventListener('focus', function () {
        this.parentElement.classList.add('focused');
    });

    input.addEventListener('
