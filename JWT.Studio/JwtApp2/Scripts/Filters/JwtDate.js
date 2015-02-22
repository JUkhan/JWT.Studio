
export default function jwtDate () {
    return function (input) {
        var len = 0;
        if (input && (len = input.length) > 8) {
            return input.substring(6, input.length - 2);
        }
        return input;
    };
};
