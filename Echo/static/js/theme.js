import { Cookies } from "/js/base.js"

const themeCookie = Cookies.get("theme");
if (!themeCookie) {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        Cookies.set("theme", "dark", 365);
    } else {
        Cookies.set("theme", "light", 365);
    }
}
