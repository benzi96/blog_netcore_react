/*
 *
 * Dashboard reducer
 *
 */
import produce from 'immer';
import htmlToDraft from 'html-to-draftjs';
import { EditorState, ContentState, convertFromRaw } from 'draft-js';
import cloneDeep from 'lodash/cloneDeep';
import {
  SET_EDITOR_STATE,
  SET_TAGS,
  SET_SUGGESTIONS,
  SET_TITLE,
  GET_MANAGE_POST_VIEW_SUCCESS,
  UPLOAD_POST_COVER_IMAGE_SUCCESS,
  UPLOAD_POST_COVER_IMAGE_ERROR,
  GET_EDIT_POST_SUCCESS,
  SET_CATEGORY_NAME,
  SET_SHORT_DESCRIPTION,
} from './constants';

export const initialState = {
  suggestions: [],
  managePostView: null,
  uploadError: null,
  post: {
    tags: [],
    title: '',
    content: EditorState.createEmpty(),
    shortDescription: '',
    categoryName: '',
    fileUploadId: null,
  },
};

/* eslint-disable default-case, no-param-reassign */
const dashboardReducer = (state = initialState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case SET_EDITOR_STATE:
        draft.post.content = action.editorState;
        break;
      case SET_TAGS:
        draft.post.tags = action.tags;
        break;
      case SET_SUGGESTIONS:
        draft.suggestions = action.suggestions;
        break;
      case SET_TITLE:
        draft.post.title = action.title;
        break;
      case SET_CATEGORY_NAME:
        draft.post.categoryName = action.categoryName;
        break;
      case SET_SHORT_DESCRIPTION:
        draft.post.shortDescription = action.shortDescription;
        break;
      case GET_MANAGE_POST_VIEW_SUCCESS:
        draft.managePostView = action.response.data;
        break;
      case UPLOAD_POST_COVER_IMAGE_SUCCESS:
        draft.post.fileUploadId = action.response.data;
        break;
      case UPLOAD_POST_COVER_IMAGE_ERROR:
        draft.uploadError = action.error;
        break;
      case GET_EDIT_POST_SUCCESS:
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
    }
  });

export default dashboardReducer;
