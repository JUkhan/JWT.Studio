
var projects = [];
function addProject() {
    var data = { name: $('#name').val(), path: $('#path').val(), allowTemplate: $('#allowTemplate')[0].checked };
    if (!data.name) {
        alert('Project name is required.');
        return;
    }
    if (!data.path) {
        alert('Path is required.');
        return;
    }
    $.post(window.location.href + '/Home/AddProject', data, function (res) {
        if (res === '101') {
            loadProjects();
            $('#name, #path').val('');
            return;
        }
        alert(res);
    });
}
function loadProject(index) {
    $.post(window.location.href + '/Home/LoadProject', projects[index], function () {
        window.location.href += '/jwtex';
    });
}
function renderTable() {
    var html = [];
    for (var i = 0; i < projects.length; i++) {
        html.push('<tr><td>' + projects[i].name + '</td><td>' + projects[i].path + '</td><td><input type="button" class="btn btn-success" value="Load Project" onclick="loadProject(' + i + ')"></td></tr>');
    }

    $('#projects').html(html.join(''));
}
function loadProjects() {
    $.get(window.location.href + '/Home/GetProjects', function (res) {
        projects = res;
        renderTable();
    });
}

$(function () {
    loadProjects();
});