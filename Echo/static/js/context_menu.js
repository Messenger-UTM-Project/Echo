class ContextIcons {
    static copy = `<svg viewBox="0 0 24 24" width="13" height="13" stroke="currentColor" stroke-width="2.5" style="margin-right: 7px" fill="none" stroke-linecap="round" stroke-linejoin="round" class="css-i6dzq1"><rect x="9" y="9" width="13" height="13" rx="2" ry="2"></rect><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"></path></svg>`;

    static cut = `<svg viewBox="0 0 24 24" width="13" height="13" stroke="currentColor" stroke-width="2.5" style="margin-right: 7px" fill="none" stroke-linecap="round" stroke-linejoin="round" class="css-i6dzq1"><circle cx="6" cy="6" r="3"></circle><circle cx="6" cy="18" r="3"></circle><line x1="20" y1="4" x2="8.12" y2="15.88"></line><line x1="14.47" y1="14.48" x2="20" y2="20"></line><line x1="8.12" y1="8.12" x2="12" y2="12"></line></svg>`;

    static paste = `<svg viewBox="0 0 24 24" width="13" height="13" stroke="currentColor" stroke-width="2.5" style="margin-right: 7px; position: relative; top: -1px" fill="none" stroke-linecap="round" stroke-linejoin="round" class="css-i6dzq1"><path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"></path><rect x="8" y="2" width="8" height="4" rx="1" ry="1"></rect></svg>`;

    static download = `<svg viewBox="0 0 24 24" width="13" height="13" stroke="currentColor" stroke-width="2.5" style="margin-right: 7px; position: relative; top: -1px" fill="none" stroke-linecap="round" stroke-linejoin="round" class="css-i6dzq1"><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path><polyline points="7 10 12 15 17 10"></polyline><line x1="12" y1="15" x2="12" y2="3"></line></svg>`;

    static delete = `<svg viewBox="0 0 24 24" width="13" height="13" stroke="currentColor" stroke-width="2.5" fill="none" style="margin-right: 7px" stroke-linecap="round" stroke-linejoin="round" class="css-i6dzq1"><polyline points="3 6 5 6 21 6"></polyline><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path></svg>`;
}

export class ContextMenu {

	static defaultMenu = [
		{
			"type" : "plain",
			"icon" : ContextIcons.copy,
			"text" : "Copy",
			"callback" : e => {navigator.clipboard.writeText(getSelectionText())},
		},
		{
			"type" : "plain",
			"icon" : ContextIcons.paste,
			"text" : "Paste"
		},
		{
			"type" : "plain",
			"icon" : ContextIcons.cut,
			"text" : "Cut",
		}
	]

	static preventStacking = () => {
		document.querySelectorAll(".context-menu").forEach(context_menu => {
			context_menu.parentElement.removeChild(context_menu);
		});
	}

	static make = list => {
		var contextMenu = document.createElement("div");
		contextMenu.classList.add("context-menu");
		contextMenu.classList.add("noselect");

		var i = 1;
		list.forEach(item => {
			var contextItem;
			
			if (item.type == "plain") {
				contextItem = document.createElement("div");
				contextItem.classList.add("context-menu-item");
				
				var item_icon = document.createElement("div");
				item_icon.classList.add("context-menu-item-icon");
				item_icon.innerHTML = item.icon;
				
				var item_text = document.createElement("div");
				item_text.classList.add("context-menu-item-text");
				item_text.innerHTML = item.text;

				contextItem.appendChild(item_icon);
				contextItem.appendChild(item_text);
			}
			else if (item.type == "split") {
				contextItem = document.createElement("div");
				contextItem.classList.add("context-menu-split");
			}
			
			contextItem.addEventListener("click", typeof item.callback == "function" ? e => item.callback(e) : undefined);
			contextItem.style.animationDelay = `${0.037*i+0.1}s`;
			contextMenu.appendChild(contextItem);
			i++;
		});
		return contextMenu;
	}

	static {
		document.body.contextMenu = ContextMenu.defaultMenu;
		window.addEventListener("contextmenu", e => {
			e.preventDefault();
			ContextMenu.preventStacking();

			var path = e.composedPath();
			var i = 0;
			while (path[i].contextMenu == undefined && path[i] != document) {
				i++;
			}

			var contextMenu = ContextMenu.make(path[i].contextMenu);
			document.body.appendChild(contextMenu);

			const { clientX, clientY } = e;

			const positionY =
			  clientY + contextMenu.scrollHeight >= window.innerHeight
				? window.innerHeight - contextMenu.scrollHeight - 20
				: clientY;
			const positionX =
			  clientX + contextMenu.scrollWidth >= window.innerWidth
				? window.innerWidth - contextMenu.scrollWidth - 20
				: clientX;

			contextMenu.setAttribute(
			  "style",
			  `--width: ${contextMenu.scrollWidth}px;
			  --height: ${contextMenu.scrollHeight}px;
			  --top: ${positionY}px;
			  --left: ${positionX}px;`
			);
		});

		window.addEventListener("click", e => {
			ContextMenu.preventStacking();
		});
	}
}

function getSelectionText() {
    var text = "";
    var activeEl = document.activeElement;
    var activeElTagName = activeEl ? activeEl.tagName.toLowerCase() : null;
    if (
      (activeElTagName == "textarea") || (activeElTagName == "input" &&
      /^(?:text|search|password|tel|url)$/i.test(activeEl.type)) &&
      (typeof activeEl.selectionStart == "number")
    ) {
        text = activeEl.value.slice(activeEl.selectionStart, activeEl.selectionEnd);
    } else if (window.getSelection) {
        text = window.getSelection().toString();
    }
    return text;
}
