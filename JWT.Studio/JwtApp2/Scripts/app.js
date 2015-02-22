
import config from 'Scripts/config.js';
import {default as controllers} from 'Scripts/app.controllers.js';
import {default as services} from 'Scripts/app.services.js';
import {default as directives} from 'Scripts/app.directives.js';
import {default as filters} from 'Scripts/app.filters.js';

var moduleName='app'; 

angular.module(moduleName,["ui.router", "ngSanitize", "ngResource",
    'ui.grid', 'ui.grid.paging', 'ui.bootstrap', controllers, services, directives, filters]).config(config);

export default moduleName;
