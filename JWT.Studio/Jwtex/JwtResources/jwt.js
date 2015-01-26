
(function (window) {
    var initializing = false, fnTest = /\b_super\b/;
    this.jsClass = function () {
        this.initFilter = function (scope) {

            if (window.sessionStorage["jwtFilter"]) {
                var ob = angular.fromJson(window.sessionStorage["jwtFilter"]);
                if (angular.isObject(ob)) {
                    for (var prop in ob) {
                        if (scope.hasOwnProperty(prop)) {
                            scope[prop] = ob[prop];
                        }
                    }
                }
            }
        };
    };
    jsClass.extend = function (prop) {
        var _super = this.prototype;
        initializing = true;
        var prototype = new this;
        initializing = false;
        for (var name in prop) {
            prototype[name] = typeof prop[name] == "function" && typeof _super[name] == "function" && fnTest.test(prop[name]) ? function (name, fn) {
                return function () {
                    var tmp = this._super;
                    this._super = _super[name];
                    var ret = fn.apply(this, arguments);
                    this._super = tmp;
                    return ret;
                }
            }(name, prop[name]) : prop[name];
        }
        function jsClass() {
            if (!initializing && this.init) {
                this.init.apply(this, arguments);
            }
        }
        jsClass.prototype = prototype;
        jsClass.prototype.constructor = jsClass;
        jsClass.extend = arguments.callee;
        return jsClass;
    };

    this.namespace = function (koPath, object) {
        var tokens = koPath.split("."), target = window;
        for (var i = 0; i < tokens.length - 1; i++) {
            target[tokens[i]] = target[tokens[i]] || {};
            target = target[tokens[i]];
        }
        target[tokens[tokens.length - 1]] = object;
    };

    namespace('jwt', {
        url: function (navName, paramValue) {
            if (this._arr.hasOwnProperty(navName)) {
                var val = this._arr[navName];
                if (val[1]) {
                    return '/#/' + val[0] + '/' + (paramValue === undefined ? 'param_value_is_undefined' : paramValue);
                }
                return '/#/' + val[0];
            }
            return "Invalid_navName:" + navName;
        }
    });
    namespace('jwt.controllers.baseCtrl', jsClass.extend({
        scope: null,
        sce:null,
        init: function (scope, sce) {
            this.scope = scope;
            this.sce = sce;
            if (toastr) {
                toastr.options.extendedTimeOut = 1000;
                toastr.options.timeOut = 1000;
                toastr.options.fadeOut = 250;
                toastr.options.fadeIn = 250;
                toastr.options.positionClass = "toast-top-right";
            }
            scope.model = {};
            scope.trustAsHtml = this.trustAsHtml.bind(this);
            scope.$on("FilterValueChange", function (e, obj) { this.onFilterValueChange(e, obj.name, obj.newValue, obj.oldValue); }.bind(this));

        },
        onFilterValueChange: function (events, filterName, newValue, oldValue) {

        },
        trustAsHtml: function (html) {
            return this.sce.trustAsHtml(html);
        },
        success: function (message) {

            toastr["success"](message);

        },
        info: function (message) {
            toastr["info"](message);
        },
        warning: function (message) {
            toastr["warning"](message);
        },
        error: function (message) {
            toastr["error"](message);
        },        
        showSpinner: function () {
            jQuery(".overlay").show();
        },
        hideSpinner: function () {
            jQuery(".overlay").hide();
        }
    }));
    //extention Methods
    Function.method = Array.method = String.method = Object.method = function (name, fn) {
        this.prototype[name] = fn;
        return this;
    };
    Array
    .method('ForEach', function (callback) {
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
    .method('Where', function (callback) {
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
    .method('First', function (callback) {

        for (var i = 0, len = this.length; i < len; i++) {
            if (callback(this[i])) { return this[i]; }
        }
        return null;
    })
    .method('Find', function (callback) {

        for (var i = 0, len = this.length; i < len; i++) {
            if (callback(this[i])) { return this[i]; }
        }
        return null;
    })
    .method('Last', function (callback) {
        var obj = null;
        for (var i = 0, len = this.length; i < len; i++) {
            if (callback(this[i])) { obj = this[i]; }
        }
        return obj;
    })
    .method('FindLast', function (callback) {
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
    .method('ToList', function () { return this; })
     .method('Insert', function (index, item) {
         return this.splice(index, 0, item);
     })
     .method('Add', function (item) { return this.push(item); })
     .method('Join', function (str) { return this.join(str); });
    Object.defineProperty(Array.prototype, 'Count', {
        get: function () {
            return this.length;
        }
    });
    Object.defineProperty(String.prototype, 'Length', {
        get: function () {
            return this.length;
        }
    });

    String
     .method('StartsWith', function (str) {
         return (this.indexOf(str) == 0);
     })
     .method('Substring', function (a, b) { return this.substring(a, b); })
     .method('ToCharArray', function () { return this; })
     .method('ToLower', function () { return this.toLowerCase(); })
     .method('ToUpper', function () { return this.toUpperCase(); })
     .method('Trim', function () { return this.trim(); })
     .method('Split', function (str) { return this.split(str); })
     .method('Replace', function (oldStr, newStr) { return this.replace(oldStr, newStr); })
     .method('IndexOf', function (str) { return this.indexOf(str); })
     .method('LastIndexOf', function (str) { return this.lastIndexOf(str); })
     .method('format', function () {
         var str = this;
         for (var i = 0; i < arguments.length; i++) {
             str = str.replace(new RegExp('\\{' + i + '\\}', 'g'), arguments[i]);
         }
         return str;
     });
    window.string = String;
    string.IsNullOrEmpty = function (str) {
        if (str) { return false; }
        return true;
    }
    string.IsNullOrWhiteSpace = function (str) {
        if (str) {
            return new RegExp("^\\s+$").test(str);
        }
        return true;
    }
    window.isValid = function (obj) {
        if (obj) { return true; }
        return false;
    }
    window.propLoop = function (obj, callback) {
        for (var prop in obj) {
            if (obj.hasOwnProperty(prop)) {
                callback(prop, obj[prop]);
            }
        }
    };

    this.SQLight = jsClass.extend({
        init: function () {
            var ops = SQLight.dbOptions;
            this.db = openDatabase(ops.fileName, ops.version, ops.displayName, ops.maxSize);
        },
        query: function (sql, paramsArray) {
            var res = {}, dbRef = this.db, scb = null, ecb = null;
            res.init = function () {
                dbRef.transaction(function (t) {
                    t.executeSql(sql, paramsArray || [], function (t, res) { if (scb) { scb(res, t); } }, function (t, res) { if (ecb) { ecb(res, t); } });
                });
                return res;
            };
            res.success = function (callback) {
                scb = callback;
                return res;
            };
            res.error = function (callback) {
                ecb = callback;
                return res;
            };
            return res.init();
        },
        getTransaction: function (callback) {
            this.db.transaction(function (t) { callback(t); });
        }
    });
    SQLight.setDbOptions = function (fileName, version, displayName, maxSize) {
        SQLight.dbOptions = { fileName: fileName, version: version, displayName: displayName, maxSize: maxSize };
    };
    SQLight.getList = function (res) {
        var arr = [];
        for (var i = 0, len = res.rows.length; i < len; i++) {
            arr.push(res.rows.item(i));
        }
        return arr;
    };
    this.Dictionary = jsClass.extend({

        Add: function (key, value) {
            this[key] = value;
        },
        ContainsKey: function (key) {
            return !!this[key];
        },
        ContainsValue: function (value) {
            for (var item in this) {
                if (this.hasOwnProperty(item) && this[item] == value) return true;
            }
            return false;
        },
        ToJsonObject: function () {
            var res = {};
            for (var item in this) {
                if (this.hasOwnProperty(item)) { res[item] = this[item]; }
            }
            return res;
        },
        ToList: function () {
            var kvList = [];
            for (var item in this) {
                if (this.hasOwnProperty(item)) { kvList.push({ Key: item, Value: this[item] }); }
            }
            return kvList;
        },
        Where: function (callback) {
            return this.ToList().Where(callback);
        },
        ForEach: function (callback) {
            this.ToList().ForEach(callback);
        },
        Remove: function (key) {
            if (this.hasOwnProperty(key)) {
                delete this[key];
            }
        },
        Clear: function () {
            for (var item in this) {
                if (this.hasOwnProperty(item)) { delete this[item]; }
            }
        }
    });
    Object.defineProperty(Dictionary.prototype, 'Keys', {
        get: function () {
            var temo = [];
            for (var key in this) {
                if (this.hasOwnProperty(key)) { temo.push(key); }
            }
            return temo;
        }
    });
    Object.defineProperty(Dictionary.prototype, 'Values', {
        get: function () {
            var temo = [];
            for (var key in this) {
                if (this.hasOwnProperty(key)) { temo.push(this[key]); }
            }
            return temo;
        }
    });
    Object.defineProperty(Dictionary.prototype, 'Count', {
        get: function () {
            var count = 0;
            for (var key in this) {
                if (this.hasOwnProperty(key)) { count++; }
            }
            return count;
        }
    });

    Function.prototype.bind = Function.prototype.bind || function (b) { if (typeof this !== "function") { throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable"); } var a = Array.prototype.slice, f = a.call(arguments, 1), e = this, c = function () { }, d = function () { return e.apply(this instanceof c ? this : b || window, f.concat(a.call(arguments))); }; c.prototype = this.prototype; d.prototype = new c(); return d; };

})(window);