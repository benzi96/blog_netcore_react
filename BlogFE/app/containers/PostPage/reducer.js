/*
 *
 * PostPage reducer
 *
 */
import produce from 'immer';
import htmlToDraft from 'html-to-draftjs';
import { EditorState, ContentState, convertFromRaw } from 'draft-js';
import cloneDeep from 'lodash/cloneDeep';
import { LOAD_POST_SUCCESS, RESET_PAGE } from './constants';

export const initialState = { post: null };

/* eslint-disable default-case, no-param-reassign */
const postPageReducer = (state = initialState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case LOAD_POST_SUCCESS:
        {
          const postData = cloneDeep(action.response.data);
          const blocksFromHtml = htmlToDraft(postData.content);
          const { contentBlocks, entityMap } = blocksFromHtml;
          const contentState = ContentState.createFromBlockArray(
            contentBlocks,
            entityMap,
          );
          postData.content = EditorState.createWithContent(contentState);
          draft.post = postData;
        }
        break;
      case RESET_PAGE:
        draft.post = null;
        break;
    }
  });

export default postPageReducer;
