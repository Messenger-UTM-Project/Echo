export class Keylogger {
	static #keys = [];
	static #list = [];
	static showKeys = false;

	static {
		window.addEventListener("mouseleave", e => this.wipe());

		window.addEventListener("keydown", e => {
			if (this.showKeys) {
				console.log(e);
			}
			this.append(e.keyCode);
		});

		window.addEventListener("keyup", e => {
			var keys = this.getKeys();
			var list = this.getList();
			var i = 0;

			if (this.showKeys) {
				console.log(e);
			}
			while (i < keys.length) {
				if (e.keyCode == keys[i]) {
					keys = this.remove(i).getKeys();
				}
				else {
					i++;
				}
			}
			list.forEach(item => {
				if (item.keyCode == e.keyCode) {
					item.func(e, item);
				}
			});
		});
	}

	static press = (keyCode, func) => {
		var info = {
			"keyCode" : keyCode,
			"func" : func
		}
		this.#list.push(info);
		return this;
	}

	static unpress = (keyCode, func) => {
		var i = 0;
		while (i < this.#list.length) {
			if (typeof func != "function") {
				if (this.#list[i].keyCode == keyCode) {
					var removed = this.#list.splice(i, 1)[0]
				}
				else {
					i++;
				}
			}
			else {
				if (this.#list[i].keyCode == keyCode && this.#list[i].func == func) {
					var removed = this.#list.splice(i, 1)[0]
				}
				else {
					i++;
				}
			}
		}
		return this;
	}

	static getKeys = () => {
		return this.#keys;
	}

	static getList = () => {
		return this.#list;
	}

	static append = (keyCode) => {
		this.#keys.push(keyCode);
		
		return this;
	}

	static wipe = () => {
		this.#keys = [];
		return this;
	}

	static remove = (i) => {
		this.#keys.splice(i, 1);
		return this;
	}
}
