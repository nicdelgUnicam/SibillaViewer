mergeInto(LibraryManager.library, {
    InitFileBrowser: function (callbackObjectName, callbackMethodName) {
        // Strings received from C# must be decoded from UTF8
        FileCallbackObjectName = UTF8ToString(callbackObjectName);
        FileCallbackMethodName = UTF8ToString(callbackMethodName);
                    
        // Create an input to take files if there isn't one already
        var filebrowser = document.getElementById('filebrowser');
        if (!filebrowser) {
            console.log('Creating filebrowser...');
            filebrowser = document.createElement('input');
            filebrowser.setAttribute('type', 'file');
            filebrowser.setAttribute('style', 'display:none;');
            filebrowser.setAttribute('id', 'filebrowser');
            filebrowser.setAttribute('class', 'nonfocused');
            document.getElementsByTagName('body')[0].appendChild(filebrowser);

            filebrowser.onchange = function (e) {
                var files = e.target.files;
                                    
                // If the dialog is cancelled, send back an empty file url
                if (files.length === 0) {
                    SendMessage(FileCallbackObjectName, FileCallbackMethodName, '');
                    return;
                }
              
                console.log('File url is ' + URL.createObjectURL(files[0]));
                SendMessage(FileCallbackObjectName, FileCallbackMethodName, URL.createObjectURL(files[0]));
            };
        }

        console.log('Filebrowser initialized!');
    },

    RequestUserFile: function (extensions) {
        // Decoding the string from UTF-8
        var str = UTF8ToString(extensions);
        var filebrowser = document.getElementById('filebrowser');
                    
        // If for some reason the filebrowser does not exist, set it
        // This can happen in Blazor.NET projects
        if (filebrowser === null)
            InitFileLoader(FileCallbackObjectName, FileCallbackMethodName);
                    
        // Set the received extensions
        if (str !== null || str.match(/^ *$/) === null)
            filebrowser.setAttribute('accept', str);
                    
        // Focus on input and click
        filebrowser.setAttribute('class', 'focused');
        filebrowser.click();
    },
            
    ResetFileBrowser: function () {
        var filebrowser = document.getElementById('filebrowser');

        if (filebrowser) {
            // Removing input from focus
            filebrowser.setAttribute('class', 'nonfocused');
        }
    },
});