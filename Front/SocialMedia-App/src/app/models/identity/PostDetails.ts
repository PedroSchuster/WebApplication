import { UserUpdate } from "./UserUpdate";

export class PostDetails {
  id: number;
  rootId: number;
  userId: number;
  user: UserUpdate;
  date: string;
  body: string;
  isLiked: boolean;
  totalLikes: number;
  totalComments: number;
  comments: PostDetails[];
  parents: PostDetails[]
}
