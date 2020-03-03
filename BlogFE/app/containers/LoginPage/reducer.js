/*
 *
 * LoginPage reducer
 *
 */
import produce from 'immer';
import {
  LOGIN_SUCCESS,
  LOGIN_ERROR,
  SET_USER_NAME,
  SET_PASSWORD,
} from './constants';

export const initialState = {
  registerMessage: null,
  userName: '',
  password: '',
};

/* eslint-disable default-case, no-param-reassign */
const loginPageReducer = (state = initialState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case LOGIN_SUCCESS:
        draft.registerMessage = action.response;
        break;
      case LOGIN_ERROR:
        break;
      case SET_USER_NAME:
        draft.userName = action.data;
        break;
      case SET_PASSWORD:
        draft.password = action.data;
        break;
    }
  });

export default loginPageReducer;
