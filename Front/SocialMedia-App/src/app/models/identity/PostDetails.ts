export class PostDetails {
  id: number;
  rootId: number;
  userId: number;
  userIcon: string | null;
  userName: string;
  date: string;
  body: string;
  comments: PostDetails[];
  parents: PostDetails[]
}
