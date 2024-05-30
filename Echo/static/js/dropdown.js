document.querySelectorAll('.dropdown').forEach(function(dropdown) {
    dropdown.addEventListener('click', function() {
		clearTimeout(dropdownTimer);
        this.setAttribute('tabindex', '1');
        this.focus();
        this.classList.toggle('active');
        var dropdownMenu = this.querySelector('.dropdown-menu');
        if (dropdownMenu.style.display === 'none') {
            dropdownMenu.style.display = 'block';
        } else {
            dropdownMenu.style.display = 'none';
        }
    });
});

let dropdownTimer = null; 
document.querySelectorAll('.dropdown').forEach(function(dropdown) {
    dropdown.addEventListener('focusout', function() {
        this.classList.remove('active');
        dropdownTimer = setTimeout(() => {this.querySelector('.dropdown-menu').style.display = 'none';}, 100);
    });
});

document.querySelectorAll('.dropdown .dropdown-menu .dropdown-item').forEach(function(item) {
    item.addEventListener('click', function() {
        var dropdown = this.closest('.dropdown');
        dropdown.querySelector('span').textContent = this.textContent;
        dropdown.querySelector('input').setAttribute('value', this.getAttribute('id'));
    });
});
