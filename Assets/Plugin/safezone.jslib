mergeInto(LibraryManager.library, {

  OpenUrl: function (str) {
	const iframe = document.createElement('iframe');
	iframe.src = UTF8ToString(str);
	iframe.style.width = '100%';
	iframe.style.height = '100vh';
	iframe.style.border = 'none';

	document.body.innerHTML = ''; // Очистить текущее содержимое
	document.body.appendChild(iframe); // Вставить iframe
  }
  
});