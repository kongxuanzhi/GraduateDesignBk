CKEDITOR.editorConfig = function (config) {
  
    config.toolbarGroups = [
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert'] },
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'tools', groups: ['tools'] },
		{ name: 'others', groups: ['others'] },
		{ name: 'about', groups: ['about'] }
    ];
    config.removeButtons = 'Save,NewPage,Print,Templates,Cut,Source,PasteText,PasteFromWord,Paste,Copy,Find,Replace,Scayt,SelectAll,Checkbox,Radio,Form,TextField,Textarea,Select,Button,ImageButton,HiddenField,Subscript,Superscript,Strike,Underline,Italic,Bold,RemoveFormat,Outdent,Indent,CreateDiv,JustifyRight,JustifyBlock,JustifyCenter,JustifyLeft,BidiLtr,BidiRtl,Language,Anchor,Unlink,Flash,Table,SpecialChar,Iframe,PageBreak,Format,Styles,BGColor,ShowBlocks,About,Font';
};