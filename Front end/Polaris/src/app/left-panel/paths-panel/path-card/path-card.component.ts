import { Component, Input, OnInit } from '@angular/core';
import { ConstValuesService } from 'src/services/const-values.service';
import { OgmaHandlerService } from 'src/services/ogma-handler.service';

@Component({
  selector: 'app-path-card',
  templateUrl: './path-card.component.html',
  styleUrls: ['./path-card.component.scss']
})
export class PathCardComponent implements OnInit {

  @Input()
  pathId : number

  @Input()
  edgeIds : string[]=[];

  public amount: number = 0;
  public show : boolean = true;
  constructor(public ogmaHandler: OgmaHandlerService , public constValues : ConstValuesService) {  }

  ngOnInit(): void {
    // this.findAmount();
  }

  public checkChange(){
    this.show =!this.show ;
    this.changeAppreanceOfEdges();
    this.changeAppreanceOfNodes();
  }
  public changeAppreanceOfEdges(){

    for(let edgeId of this.edgeIds){

      this.ogmaHandler.ogma.getEdge(edgeId).setAttributes(
        {color :  this.show ? this.constValues.inPathEdgeColor : this.constValues.standardEdgeColor });

   }
  }
  public changeAppreanceOfNodes(){
  }
}
