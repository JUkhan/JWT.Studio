
import config from 'Scripts/config.js';
import {default as appControllers} from 'Scripts/App.Controllers.js';
import {default as services} from 'Scripts/App.Services.js';

var moduleName='app'; 

angular.module(moduleName,["ui.router", "ngSanitize", "ngResource", 'ui.grid', 'ui.grid.paging', 'ui.bootstrap', appControllers,services]).config(config);

export default moduleName;
