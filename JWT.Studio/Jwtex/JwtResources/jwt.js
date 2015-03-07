
(function (window) {
   
    //extention Methods
    Function.method = Array.method = String.method = Object.method = function (name, fn) {
        this.prototype[name] = fn;
        return this;
    };
    Array
    .method('each', function (callback) {
        for (var i = 0, len = this.length; i < len; i++) {
            callback(this[i], i);
        }
    })
    .method('remove', function (callback) {
        var fx = function (arr) { return arr.length; };
        for (var i = 0; i < fx(this) ; i++) {
            if (callback(this[i])) { this.splice(i, 1); i--; }
        }
        return this;
    })
    .method('where', function (callback) {
        var res = [];
        for (var i = 0, len = this.length; i < len; i++) {
            if (callback(this[i])) { res.push(this[i]); }
        }
        return res;
    })
    .method('select', function (conditionalCallback, selectionCallback) {
        var res = [];
        for (var i = 0, len = this.length; i < len; i++) {
            if (conditionalCallback(this[i])) { res.push(selectionCallback(this[i])); }
        }
        return res;
    })
    .method('first', function (callback) {

        for (var i = 0, len = this.length; i < len; i++) {
            if (callback(this[i])) { return this[i]; }
        }
        return null;
    })   
    .method('last', function (callback) {
        var obj = null;
        for (var i = 0, len = this.length; i < len; i++) {
            if (callback(this[i])) { obj = this[i]; }
        }
        return obj;
    })
    .method('selectWithJoin', function (list1, list1Callback, conditionalCallback, selectionCallback) {
        var res = [], fx = function (list, pitem, callback) {
            var temp = [];
            for (var i = 0, len = list.length; i < len; i++) {
                if (callback(pitem, list[i])) { temp.push(list[i]); }
            }
            return temp;
        };
        this.ForEach(function (item) {
            fx(list1, item, list1Callback).ForEach(function (join) {
                if (conditionalCallback(item, join)) {
                    res.push(selectionCallback(item, join));
                }
            });
        });
        return res;
    })
    .method('groupBy', function (callback) {
        var res = [], temp = [];
        this.ForEach(function (item) {
            var key = callback(item);
            if (temp[key]) {
                temp[key].push(item);
            } else {
                temp[key] = [item];
            }
        });
        for (var i in temp) {
            if (temp.hasOwnProperty(i)) {
                res.push({ key: i, items: temp[i] });
            }
        }
        return res;
    })
    .method('paging', function (pageno, size) {
        pageno--;
        return this.slice(pageno * size, (pageno * size) + size);
    })   
     .method('insertAt', function (index, item) {
         return this.splice(index, 0, item);
     });
     
    String.method('format', function () {
         var str = this;
         for (var i = 0; i < arguments.length; i++) {
             str = str.replace(new RegExp('\\{' + i + '\\}', 'g'), arguments[i]);
         }
         return str;
     });    
    String.isNullOrWhiteSpace = function (str) {
        if (str) {
            return new RegExp("^\\s+$").test(str);
        }
        return true;
    }   
   
    Function.prototype.bind = Function.prototype.bind || function (b) { if (typeof this !== "function") { throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable"); } var a = Array.prototype.slice, f = a.call(arguments, 1), e = this, c = function () { }, d = function () { return e.apply(this instanceof c ? this : b || window, f.concat(a.call(arguments))); }; c.prototype = this.prototype; d.prototype = new c(); return d; };

})(window);

