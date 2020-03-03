/*
 *
 * HomePage reducer
 *
 */
import produce from 'immer';
import {
  DEFAULT_ACTION,
  GET_LIST_POST_VIEW_SUCCESS,
  RESET_PAGE,
} from './constants';

export const initialState = {
  listPostView: {
    posts: [],
    hasMore: true,
  },
};

/* eslint-disable default-case, no-param-reassign */
const homePageReducer = (state = initialState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case DEFAULT_ACTION:
        break;
      case GET_LIST_POST_VIEW_SUCCESS:
        draft.listPostView.posts = draft.listPostView.posts.concat(
          action.response.data.posts,
        );
        draft.listPostView.hasMore = action.response.data.hasMore;
        break;
      case RESET_PAGE:
        draft.listPostView = {
          posts: [],
          hasMore: true,
        };
        break;
    }
  });

export default homePageReducer;
