document.querySelectorAll('.dropdown').forEach(function(dropdown) {
    dropdown.addEventListener('click', function(e) {
        this.setAttribute('tabindex', '1');
        this.classList.toggle('active');
        this.focus();
        var dropdownMenu = this.querySelector('.dropdown-menu');
        if (dropdownMenu.style.display === 'none') {
            dropdownMenu.style.display = 'block';
        } else {
            dropdownMenu.style.display = 'none';
        }
    });
});

document.querySelectorAll('.dropdown').forEach(function(dropdown) {
    dropdown.addEventListener('focusout', function(e) {
		this.classList.remove('active');
		this.querySelector(".dropdown-menu").style.display = 'none';
    });

	dropdown.querySelectorAll('.dropdown-menu a').forEach(function(link) {
        link.addEventListener('mousedown', function(e) {
            e.preventDefault();
            setTimeout(() => {
                window.location.href = this.href;
            }, 0);
        });
    });
});

document.querySelectorAll('.dropdown .dropdown-menu .dropdown-item').forEach(function(item) {
    item.addEventListener('click', function(e) {
        var dropdown = this.closest('.dropdown');
        dropdown.querySelector('span').textContent = this.textContent;
        dropdown.querySelector('input').setAttribute('value', this.getAttribute('id'));
    });
});
