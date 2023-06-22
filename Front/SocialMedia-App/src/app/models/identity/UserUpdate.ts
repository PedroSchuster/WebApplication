export class UserUpdate {
  id: number;
  userName: string = '';
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  phoneNumber: string = '';
  bio: string = '';
  profilePicURL: string = '';
  password: string = '';
  token: string = '';
  followers: UserUpdate[];
  following: UserUpdate[];
  followingCount: number;
  followersCount: number;
}
