export class PostDetails {
  id: number;
  rootId: number;
  userId: number;
  userIcon: string | null;
  userName: string;
  date: string;
  body: string;
  totalLikes: number;
  totalComments: number;
  comments: PostDetails[];
  parents: PostDetails[]
}
