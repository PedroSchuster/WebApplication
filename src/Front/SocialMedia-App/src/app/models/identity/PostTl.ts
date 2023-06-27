import { PostDetails } from "./PostDetails";
import { UserUpdate } from "./UserUpdate";

export class PostTL {
  id: number;
  userId: number;
  user: UserUpdate;
  date: string;
  body: string;
  isLiked: boolean;
  totalLikes: number;
  totalComments: number;
}
