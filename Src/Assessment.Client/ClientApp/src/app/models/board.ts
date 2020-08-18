import { PostIt } from "./post-it";

export class Board {
  public id: number;
  public name: string;
  public createdAt: Date;
  public postIts: PostIt[];

}
