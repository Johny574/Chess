mergeInto(LibraryManager.library, {
  InitStockfish: function () {
    if (typeof window.stockfish === 'undefined') {
      try {
        //console.log("[Stockfish.jslib] Attempting to load worker...");
        window.stockfish = new Worker('Stockfish/stockfish.js');
        window.sfOutput = '';

        window.stockfish.onmessage = function (event) {
          //console.log("[Stockfish Worker] says:", event.data);
          window.sfOutput += event.data + '\n';
        };

        //console.log("[Stockfish.jslib] Worker initialized");
      } catch (e) {
        //console.error("[Stockfish.jslib] Worker failed to load:", e);
        window.stockfish = null;
      }
    }
  },

  SendCommandToStockfish: function (cmdPtr) {
    var cmd = UTF8ToString(cmdPtr);
    if (window.stockfish) {
      //console.log("[Stockfish.jslib] Sending command:", cmd);
      window.stockfish.postMessage(cmd);
    } else {
      //console.warn("[Stockfish.jslib] Cannot send command—worker not initialized.");
    }
  },

  GetOutputFromStockfish: function () {
    var output = window.sfOutput || '';
    window.sfOutput = ''; // Clear buffer after read
    var lengthBytes = lengthBytesUTF8(output) + 1;
    var buffer = _malloc(lengthBytes);
    stringToUTF8(output, buffer, lengthBytes);
    return buffer;
  }
});