(function (global, undefined) {
    var EditorCommandList;

    var registerCustomCommands = function () {
        EditorCommandList["InsertFrontDoubleQuote"] = function (commandName, editor, args) {
            editor.pasteHtml('&ldquo;');
        };
        EditorCommandList["InsertBackDoubleQuote"] = function (commandName, editor, args) {
            editor.pasteHtml('&rdquo;');
        };
        EditorCommandList["InsertDash"] = function (commandName, editor, args) {
            editor.pasteHtml('&ndash;');
        };

        EditorCommandList["SaraEaar"] = function (commandName, editor, args) {
            editor.pasteHtml('<span style="font-family:Wp Thai wannayuk a;font-size: 46px;">&nbsp;เอียะ</span>');
            };
            EditorCommandList["SaraA"] = function (commandName, editor, args) {
                editor.pasteHtml('<span style="font-family:Wp Thai wannayuk a;">&nbsp;อะ</span>');
            };

            EditorCommandList["Sara15"] = function (commandName, editor, args) {
                editor.pasteHtml('<span style="font-family:Wp Thai wannayuk a;">&nbsp;เอาะ</span>');
            };

    };

    global.TelerikDemo = {
        editor_onClientLoad: function (editor, args) {
            EditorCommandList = Telerik.Web.UI.Editor.CommandList;
            registerCustomCommands();
        }
    };

})(window);

