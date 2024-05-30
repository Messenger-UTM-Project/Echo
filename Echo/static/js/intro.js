import { Cookies } from "/js/base.js"

let intro = document.querySelector(".intro");
if (intro)
{
	Cookies.set("intro", 1, 20);

	// Start intro animations when fully loaded.
	window.addEventListener("load", e =>
	{
		intro.setAttribute(
		  "style",
		  "visibility: hidden; opacity: 0"
		);
	});
}
