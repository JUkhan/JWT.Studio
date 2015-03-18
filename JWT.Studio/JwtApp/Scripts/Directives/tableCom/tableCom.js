let COMPILE=new WeakMap();
class tableCom
{
	constructor(compile){
      	COMPILE.set(this, compile);
		this.restrict='A';		
      	this.scope={
          data:'='
        };
	}
  
  	link(scope, iElement, attr){      
      	
      	scope.$watch('data', function(){
          tableCom.instance.render(scope, iElement);
        });      	
      
      	tableCom.instance.render(scope, iElement);
    }
  	render(scope, element){
      element.empty().append(COMPILE.get(this)(this.getTemplate(scope.data))(scope));
    }
  	getTemplate(data){
      var tpl=[];
      if(angular.isArray(data) && data.length>0){
          
          	tpl.push('<table class="table table-bordered table-striped">');
            //thead         
            tpl.push('<thead><tr><th>#</th>');
        	for(var prop in data[0]){
              tpl.push('<th>'+prop+'</th>')
            }
          	tpl.push('</tr></thead>');
          	//tbody
          	tpl.push('<tbody><tr ng-animate="\'animate\'" ng-repeat="item in data"><td ng-bind="$index+1"></td>');
        	for(var prop in data[0]){
              tpl.push('<td ng-bind="item[\''+prop+'\']"></td>')
            }
          	tpl.push('</tr></tbody>');
          
            tpl.push('</table>')         
        }
      	else{
          tpl.push('<b>Data not found.<b>')
        }
      	return tpl.join('');
    }
	static builder(compile){
      	tableCom.instance=new tableCom(compile);
		return tableCom.instance;
	}
}
tableCom.builder.$inject=['$compile'];
export default tableCom;