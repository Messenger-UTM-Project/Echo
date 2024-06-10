document.querySelectorAll(".editProfilePicture").forEach(item => {
	item.addEventListener('click', e => {
		var edit = document.querySelector(".profile-image-edit-container");
		edit.classList.toggle("hidden");
	});
});

document.querySelector(".profile-image-edit-container").addEventListener('click', e => {
	var edit = document.querySelector(".profile-image-edit-container");
	if (e.target == edit) {
		edit.classList.toggle("hidden");
	}
});
