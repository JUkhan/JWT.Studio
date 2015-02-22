class DbWeatherCtrl
{
	constructor(scope){
		this.title='DbWeather update';
      	scope.list=[
          {id:1, title:'Dhaka' },
          {id:2, title:'Chitagong' },
          {id:3, title:'Cox' }
        ];
	}
}
DbWeatherCtrl.$inject=['$scope'];
export default DbWeatherCtrl;