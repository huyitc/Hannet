export class TreeAZoneTDevice {
  id!: number;
  name!: string;
  childrens?: TreeAZoneTDevice[];
  constructor(id: number, name: string, childrens: TreeAZoneTDevice[]) {
    this.id = id;
    this.name = name;
    this.childrens = childrens;
  }
}