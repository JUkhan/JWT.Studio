
import config from 'Scripts/config.js';
import {default as appControllers} from 'Scripts/App.Controllers.js';

var moduleName='app'; 

angular.module(moduleName,['ui.router', appControllers]).config(config);

export default moduleName;
