var resolve={
	
	test:function(testData){
	    
		return testData.data;
	},
	testData:function($stateParams, widget1Svc){
	    
	   return widget1Svc.getTestData();
	
	
	}
}