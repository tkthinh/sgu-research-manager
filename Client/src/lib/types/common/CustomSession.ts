import { Session } from "react-router-dom";
import { User } from "../models/User";

export interface CustomSession extends Session {
  user: User;
}