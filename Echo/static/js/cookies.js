export class Cookies {
	static set = (cname, cvalue, exmin) => {
		try {
			var d = new Date();
			d.setTime(d.getTime() + (exmin*60*1000));
			var expires = "expires="+ d.toUTCString();
			document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/; SameSite=Strict; Secure";}
		catch {
			return false;
		}
		return true;
	}

	static get = cname => {
		var name = cname + "=";
		var ca = document.cookie.split(';');
		
		for(var i = 0; i < ca.length; i++) {
			var c = ca[i];
			
			while (c.charAt(0) == ' ') {
				c = c.substring(1);
			}
			
			if (c.indexOf(name) == 0) {
				return c.substring(name.length, c.length);
			}
		}
		return null;
	}
}
