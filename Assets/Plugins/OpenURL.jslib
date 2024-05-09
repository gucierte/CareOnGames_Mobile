var NPlugin = {
	OpenURL: function (url) {
		window.open(UTF8ToString(url), "_blank");
	},

	OpenURLInSameTab: function (url) {
		window.open(UTF8ToString(url), "_self");
	}
};

mergeInto(LibraryManager.library, NPlugin);